import React, {useState} from "react";
import {useNavigate} from "react-router-dom";
import {useMutation, useQuery} from "react-query";
import {ccFetch} from "../utitilies";

type User = {
    id: string;
    name: string;
    role: string;
}

export default function AdminUsers() {
    const navigate = useNavigate();
    const [users, setUsers] = useState([] as User[]);

    useQuery(["adminUserData"], {
        queryFn: () => {
            return ccFetch("/api/User", "GET", navigate);
        },
        onSuccess: async (data: Response) => {
            setUsers(await data.json() as User[]);
        },
        onError: (e) => {
            console.error(e);
        }
    });

    function onCreateUser() {
        console.log("hello");
        navigate("/admin/users/new");
    }

    return (
        <>
            <button onClick={() => onCreateUser()}>New User</button>
            <table className="table-auto border border-slate-400 text-left w-full">
                <thead>
                <tr className="border">
                    <th className="px-3">Username</th>
                    <th className="px-3">Role</th>
                    <th className="px-3"><span className="sr-only">Edit</span></th>
                </tr>
                </thead>

                <tbody>
                    {users.map((user) => <UserRow key={user.id} user={user}/>)}
                </tbody>
            </table>
        </>
    );
}

function UserRow({ user }: { user: User }) {
    const [editing, setEditing] = useState(false);
    const [deleted, setDeleted] = useState(false);
    const [username, setUsername] = useState(user.name);
    const [role, setRole] = useState(user.role);
    
    const saveMutation = useMutation({
        mutationFn: async () => {
            return await fetch(`/api/User/${user.id}`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    id: user.id,
                    name: username,
                    role: role
                })
            })
        },
        onSuccess: () => {
            setEditing(false);
        }
    });

    const deleteMutation = useMutation({
        mutationFn: async () => {
            return await fetch(`/api/User/${user.id}`, {
                method: "DELETE"
            })
        },
        onSuccess: () => {
            setDeleted(true);
        }
    });
    
    function onSave() {
        saveMutation.mutate();
    }
    
    function onDelete() {
        deleteMutation.mutate();
    }
    
    if (deleted) {
        return <></>
    }
    
    if (editing) {
        return (
            <tr className="border">
                <td className="px-3"><input defaultValue={username} onChange={(e) => setUsername(e.target.value)} /></td>
                <td className="px-3">
                    <select defaultValue={role} onChange={(e) => setRole(e.target.value)}>
                        <option value="Admin">Admin</option>
                        <option value="Normal">Normal</option>
                    </select>
                </td>
                <td className="px-3">
                    <span className="text-green-600 hover:underline cursor-pointer"
                          onClick={() => onSave()}>
                        Save
                    </span>
                    <span className="text-red-600 hover:underline cursor-pointer"
                          onClick={() => onDelete()}>
                        Remove
                    </span>
                </td>
            </tr>
        );
    } else {
        return (
            <tr className="border">
                <td className="px-3">{username}</td>
                <td className="px-3">{role}</td>
                <td className="px-3">
                    <span className="text-blue-600 hover:underline cursor-pointer"
                          onClick={() => setEditing(true)}>
                        Edit
                    </span>
                </td>
            </tr>
        );
    }
}