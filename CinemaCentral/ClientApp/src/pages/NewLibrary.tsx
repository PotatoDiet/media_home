import React, {FormEvent, useState} from "react";
import {useNavigate} from "react-router-dom";
import {useMutation} from "react-query";

export default function NewLibrary() {
    const navigate = useNavigate();
    const [name, setName] = useState("");
    const [root, setRoot] = useState("");

    const saveMutation = useMutation("newLibrary", {
        mutationFn: () => {
            return fetch(`/api/Library`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    name: name,
                    root: root,
                })
            })
        },
        onSuccess: () => {
            navigate("/admin/libraries");
        }
    });

    function handleSubmit(e: FormEvent<HTMLFormElement>) {
        e.preventDefault();
        saveMutation.mutate();
    }

    return (
        <form onSubmit={(e) => handleSubmit(e)}>
            <label>
                Name
                <input type="text" onChange={(e) => setName(e.target.value)}></input>
            </label>

            <label>
                Root
                <input type="text" onChange={(e) => setRoot(e.target.value)}></input>
            </label>

            <input type="submit" value="Create"></input>
        </form>
    )
}