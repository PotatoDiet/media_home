import React from "react";
import {Link, Outlet} from "react-router-dom";

function Sidebar() {
    return (
        <aside aria-label="Sidebar">
            <div className="overflow-y-auto py-4 px-3 rounded">
                <ul className="space-y-2">
                    <li>
                        <a href="#"
                           className="flex items-center p-2 text-base font-normal text-slate-900 rounded-lg hover:bg-slate-200">
                            <svg aria-hidden="true"
                                 className="flex-shrink-0 w-6 h-6 text-slate-300 transition duration-75 group-hover:text-gray-900"
                                 fill="currentColor" viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg">
                                <path fillRule="evenodd" d="M10 9a3 3 0 100-6 3 3 0 000 6zm-7 9a7 7 0 1114 0H3z"
                                      clipRule="evenodd"></path>
                            </svg>
                            <Link to="/admin/users" className="ml-3">Users</Link>
                        </a>
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