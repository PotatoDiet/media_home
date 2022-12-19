import React, {useCallback, useEffect, useState} from "react";
import {Link, Outlet, useLocation, useNavigate} from "react-router-dom";
import {useQuery} from "react-query";
import {ccFetch} from "../utitilies";

type User = {
    role: "Normal" | "Admin"
}

function SidebarEntry(props: any) {
    const location = useLocation();
    const onPage = location.pathname === props.to;
    
    return (
        <li>
            <Link {...props}
                  className={`flex items-center py-2 px-5 text-base font-normal text-slate-900 rounded-lg hover:bg-slate-200 ${onPage && "bg-slate-200"}`} />
        </li>
    );
}

function Sidebar() {
    const navigate = useNavigate();
    const location = useLocation();
    const [search, setSearch] = useState("");
    const [isAdmin, setIsAdmin] = useState(false);

    useEffect(() => {
        const searchQuery = new URLSearchParams(location.search).get("search");
        setSearch(searchQuery ?? "");
    }, [location]);
    
    useQuery("checkAdmin", {
        queryFn: () => {
            return ccFetch("/api/User/Current", "GET", navigate);
        },
        onSuccess: async (data: Response) => {
            const user: User = await data.json();
            if (user.role == "Admin") {
                setIsAdmin(true);
            }
        }
    });

    const logout = useCallback(async () => {
        await fetch("/api/User/Logout", {
            method: "POST"
        });
        navigate("/login");
    }, [navigate]);

    const onSearch = (event: React.KeyboardEvent<HTMLInputElement>) => {
        // onSearch doesn't work on firefox, so this is the best we can do.
        if (event.key === 'Enter') {
            const target = event.target as HTMLInputElement;

            if (target.value === 'enter') {
                navigate('/movies');
            } else {
                navigate(`/movies?search=${target.value}`);
            }
        }
    };
    
    return (
        <ul className="basis-72 flex-none">
            <SidebarEntry to="/">Home</SidebarEntry>
            <SidebarEntry to="/movies">Movies</SidebarEntry>
            <SidebarEntry to="#" onClick={() => logout()}>Logout</SidebarEntry>

            {isAdmin &&
                <>
                    <SidebarEntry to="/admin/users">Users</SidebarEntry>
                    <SidebarEntry to="/admin/libraries">Libraries</SidebarEntry>
                </>
            }

            <input type="search" placeholder="Search" onKeyUp={onSearch} defaultValue={search} />
        </ul>
    );
}

export default function Layout() {
    return (
        <div className="flex">
            <Sidebar />

            <div className="grow p-3">
                <Outlet />
            </div>
        </div>
    );
}