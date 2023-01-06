import {ccFetch} from "../utitilies";
import {useNavigate} from "react-router-dom";
import React, {useState} from "react";
import {useMutation, useQuery} from "react-query";

type Library = {
    id: string;
    name: string;
    root: string;
}

export default function AdminLibraries() {
    const navigate = useNavigate();
    const [libraries, setLibraries] = useState([] as Library[]);
    
    async function update() {
        await ccFetch("/api/Library/Update", "POST", navigate);
    }
    
    async function onCreateLibrary() {
        navigate("/admin/libraries/new");
    }

    useQuery(["adminLibraryData"], {
        queryFn: () => {
            return ccFetch("/api/Library/ListLibraries", "GET", navigate);
        },
        onSuccess: async (data: Response) => {
            setLibraries(await data.json());
        }
    });
    
    return (
        <>
            <button type="button"
                    onClick={update}
                    className="p-3 mb-3 border rounded-md hover:bg-slate-200">
                Scan Libraries
            </button>

            <button type="button"
                    onClick={onCreateLibrary}
                    className="p-3 mb-3 ml-3 border rounded-md hover:bg-slate-200">
                New Library
            </button>

            <table className="table-auto border border-slate-400 text-left w-full">
                <thead>
                <tr className="border">
                    <th className="px-3">Name</th>
                    <th className="px-3">Root</th>
                    <th className="px-3"><span className="sr-only">Edit</span></th>
                </tr>
                </thead>

                <tbody>
                    {libraries.map((x) => <LibraryRow key={x.id} library={x}/>)}
                </tbody>
            </table>
        </>
    )
}

function LibraryRow({ library }: { library: Library}) {
    const [name, setName] = useState(library.name);
    const [root, setRoot] = useState(library.root);
    const [editing, setEditing] = useState(false);
    const [deleted, setDeleted] = useState(false);

    const saveMutation = useMutation({
        mutationFn: async () => {
            return await fetch(`/api/Library/${library.id}`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    id: library.id,
                    name: name,
                    root: root
                })
            })
        },
        onSuccess: () => {
            setEditing(false);
        }
    });

    const deleteMutation = useMutation({
        mutationFn: async () => {
            return await fetch(`/api/Library/${library.id}`, {
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
    
    if (deleted) return <></>;

    if (editing) {
        return (
            <tr className="border">
                <td className="px-3"><input defaultValue={name} onChange={(e) => setName(e.target.value)}/></td>
                <td className="px-3"><input defaultValue={root} onChange={(e) => setRoot(e.target.value)}/></td>
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
                <td className="px-3">{name}</td>
                <td className="px-3">{root}</td>
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