import React, {FormEvent, useState} from "react";
import {useNavigate} from "react-router-dom";

export default function Login() {
    const navigate = useNavigate();
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    
    async function handleSubmit(e: FormEvent) {
        e.preventDefault();
        
        const response = await fetch("/api/User/Login", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                "name": username,
                "password": password
            })
        })
        if (response.status === 200) {
            navigate("/");
        }
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
            
            <input type="submit" value="Login"></input>
        </form>
    )
}