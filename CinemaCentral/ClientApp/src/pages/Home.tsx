import React, {useEffect, useState} from "react";
import MediaTile from "../components/MediaTile";
import {ccFetch} from "../utitilies";
import {json, useLocation, useNavigate} from "react-router-dom";
import {useQuery} from "react-query";

type Media = {
    id: string;
    posterPath: string;
    mediaType: string;
}

export default function Home() {
    const navigate = useNavigate();
    const { search } = useLocation();
    
    const { isLoading, data, refetch } = useQuery("mediaData", async () => {
        const response = await ccFetch(`/api/Media${search}`, "GET", navigate);
        return await response.json();
    });

    useEffect(() => {
        refetch();
    }, [search])
    
    if (isLoading) return <>Loading...</>;
    
    return (
        <div className="flex flex-wrap">
            {data.map((v: Media) => (
                <MediaTile
                    key={v.id}
                    id={v.id}
                    type={v.mediaType}
                    poster={v.posterPath}
                />
            ))}
        </div>
    );
};
