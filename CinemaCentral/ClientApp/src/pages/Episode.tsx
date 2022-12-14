import React from "react";
import {useParams} from "react-router-dom";
import EpisodePlayer from "../components/EpisodePlayer";

export default function Episode() {
    const params = useParams();
    
    return (
        <EpisodePlayer id={params.id ?? ""}></EpisodePlayer>
    )
}