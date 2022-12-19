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
        <Link {...props}
              className={`flex items-center py-2 px-5 text-base font-normal text-slate-900 hover:bg-slate-200 ${onPage && "bg-slate-200"}`} />
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
    }, [location.search, location.pathname]);
    
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

            if (target.value === "") {
                navigate(location.pathname);
            } else {
                navigate(`${location.pathname}?search=${target.value}`);
            }
        }
    };
    
    return (
        <div className="basis-72 flex-none divide-y border-x h-screen">
            <div>
                <input className="h-10 py-2 w-full px-5" type="search" placeholder="Search" onKeyUp={onSearch} onChange={(e) => setSearch(e.target.value)} value={search} />
            </div>
            
            <div>
                <div className="py-2 px-5 text-center text-sm font-bold text-slate-900">Libraries</div>
                <SidebarEntry to="/">All</SidebarEntry>
                <SidebarEntry to="/movies">Movies</SidebarEntry>
                <SidebarEntry to="/tv">TV</SidebarEntry>
            </div>

            {isAdmin &&
                <div>
                    <div className="py-2 px-5 text-center text-sm font-bold text-slate-900">Admin</div>
                    <SidebarEntry to="/admin/users">Users</SidebarEntry>
                    <SidebarEntry to="/admin/libraries">Libraries</SidebarEntry>
                </div>
            }

            <div>
                <div className="py-2 px-5 text-center text-sm font-bold text-slate-900">Profile</div>
                <SidebarEntry to="#" onClick={() => logout()}>Logout</SidebarEntry>
            </div>
        </div>
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