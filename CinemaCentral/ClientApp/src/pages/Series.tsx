import {useParams} from "react-router-dom";
import React, {useEffect, useState} from "react";
import EpisodeTile from "../components/EpisodeTile";

type Episode = {
    id: string;
    posterPath: string;
    title: string;
}

export default function Series() {
    const params = useParams();
    
    let [episodes, setEpisodes] = useState([]);

    useEffect( () => {
        async function grabVideos() {
            const res = await fetch(`/api/Series/GetSeries/${params.id}`);
            const data = await res.json();
            setEpisodes(data["episodes"])
        }
        grabVideos();
    }, [window.location.search]);
    
    return (
        <div className="flex flex-wrap">
            {episodes.map((v: Episode) => (
                <EpisodeTile
                    key={v.id}
                    id={v.id}
                    poster={v.posterPath}
                    title={v.title}
                />
            ))}
        </div>
    );
}