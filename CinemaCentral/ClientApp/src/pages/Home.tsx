import React, {useEffect, useState} from "react";
import MediaTile from "../components/MediaTile";
import './Movies.css';
import {ccFetch} from "../utitilies";
import {json, useNavigate} from "react-router-dom";
import {useQuery} from "react-query";

type Media = {
    id: string;
    posterPath: string;
    mediaType: string;
}

export default function Home() {
    const navigate = useNavigate();
    
    const { isLoading, error, data } = useQuery("mediaData", async () => {
        const response = await ccFetch(`/api/Media${window.location.search}`, "GET", navigate);
        return await response.json();
    });
    
    if (isLoading) return <>Loading...</>;
    
    return (
        <span className="videos">
            {data.map((v: Media) => (
                <MediaTile
                    key={v.id}
                    id={v.id}
                    type={v.mediaType}
                    poster={v.posterPath}
                />
            ))}
        </span>
    );
};
