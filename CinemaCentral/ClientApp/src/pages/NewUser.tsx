import React, {FormEvent, useState} from "react";
import {useMutation} from "react-query";
import {useNavigate} from "react-router-dom";

export function NewUser() {
    const navigate = useNavigate();
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [role, setRole] = useState("Normal");

    const saveMutation = useMutation("newUser", {
        mutationFn: () => {
            return fetch(`/api/User/Create`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    name: username,
                    password: password,
                    role: role
                })
            })
        },
        onSuccess: () => {
            navigate("/admin/users");
        }
    });

    function handleSubmit(e: FormEvent<HTMLFormElement>) {
        e.preventDefault();
        saveMutation.mutate();
    }

    return (
        <form onSubmit={(e) => handleSubmit(e)}>
            <label>
                Username
                <input type="text" onChange={(e) => setUsername(e.target.value)}></input>
            </label>
    
            <label>
                Password
                <input type="text" onChange={(e) => setPassword(e.target.value)}></input>
            </label>

            <select value={role} onChange={(e) => setRole(e.target.value)}>
                <option value="Admin">Admin</option>
                <option value="Normal">Normal</option>
            </select>
    
            <input type="submit" value="Create"></input>
        </form>
    );
}