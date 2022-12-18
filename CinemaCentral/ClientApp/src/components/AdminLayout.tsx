import React from "react";
import {Link, Outlet} from "react-router-dom";

function Sidebar() {
    return (
        <aside aria-label="Sidebar">
            <div className="overflow-y-auto py-4 px-3 rounded">
                <ul className="space-y-2">
                    <li>
                        <Link to="/"
                              className="ml-3 flex items-center p-2 text-base font-normal text-slate-900 rounded-lg hover:bg-slate-200">
                            Home
                        </Link>
                    </li>
                    
                    <li>
                        <Link to="/admin/users"
                              className="ml-3 flex items-center p-2 text-base font-normal text-slate-900 rounded-lg hover:bg-slate-200">
                            Users
                        </Link>
                    </li>
                </ul>
            </div>
        </aside>
    )
}

export default function AdminLayout() {
    return (
        <section className="w-full max-w-7xl mx-auto p-5 m-5 border-2">
            <div className="grid md:grid-cols-12 gap-5 p-4 m-2">
                <div className="md:col-span-3 md:pt-0 p-2">
                    <Sidebar />
                </div>
                
                <div className="md:col-span-9 p-4 border-2">
                    <Outlet />
                </div>
            </div>
        </section>
    )
}