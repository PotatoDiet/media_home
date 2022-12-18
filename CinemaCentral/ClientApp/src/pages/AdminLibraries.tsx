import {ccFetch} from "../utitilies";
import {useNavigate} from "react-router-dom";
import React from "react";

export default function AdminLibraries() {
    const navigate = useNavigate();
    
    async function update() {
        await Promise.all([
            ccFetch("/api/Movies/Update", "POST", navigate),
            ccFetch("/api/Series/Update", "POST", navigate)
        ]);
    }
    
    return (
        <button type="button" onClick={update}>Update</button>
    )
}