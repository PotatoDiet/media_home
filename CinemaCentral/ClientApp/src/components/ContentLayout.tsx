import React from "react";
import Navbar from "./Navbar";
import {Outlet} from "react-router-dom";

export default function ContentLayout() {
    return (
        <>
            <Navbar />
            <div className="content">
                <Outlet />
            </div>
        </>
    )
}