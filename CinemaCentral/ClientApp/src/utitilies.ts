import {json, NavigateFunction, redirect} from "react-router-dom";

export type HttpMethod = "GET" | "POST" | "DELETE";

export async function ccFetch(url: string, method: HttpMethod, navigate: NavigateFunction): Promise<any> {
    const response = await fetch(url, {
        method: method,
        headers: {
            "Content-Type": "application/json"
        },
    });
    
    if (response.status === 401) {
        navigate("/login");
    } else {
        return response;
    }
}