import {ccFetch} from "../utitilies";
import {useNavigate} from "react-router-dom";
import React from "react";

export default function AdminLibraries() {
    const navigate = useNavigate();
    
    async function update() {
        await ccFetch("/api/Library/Update", "POST", navigate)
    }
    
    return (
        <button type="button"
                onClick={update}
                className="p-3 mb-3 border rounded-md hover:bg-slate-200">
            Update
        </button>
    )
}