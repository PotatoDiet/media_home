import React, {useEffect, useState} from "react";
import {useNavigate, useParams} from "react-router-dom";
import EpisodePlayer from "../components/EpisodePlayer";
import {ccFetch} from "../utitilies";

export default function Episode() {
    const { id } = useParams();
    const navigate = useNavigate();
    const [currentWatchTimestamp, setCurrentWatchTimestamp] = useState(0);
    const [isLoaded, setIsLoaded] = useState(false);

    useEffect(() => {
        const grabData = async () => {
            const res = await ccFetch(`/api/Series/GetWatchtimeStamp/${id}`, "GET", navigate);
            setCurrentWatchTimestamp(await res.json());
            setIsLoaded(true);
        };

        grabData();
    }, []);
    
    if (!isLoaded) {
        return <></>
    }
    
    return (
        <EpisodePlayer id={id!} watchtime={currentWatchTimestamp}></EpisodePlayer>
    )
}