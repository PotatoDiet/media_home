import {Link} from "react-router-dom";
import React from "react";

type EpisodeTileProps = {
    id: string;
    poster: string;
    title: string;
}

export default function EpisodeTile({ id, poster, title }: EpisodeTileProps) {
    return (
        <Link to={`/episode/${id}`} className="w-80 m-1">
            <img src={poster} alt={title}/>
            <div className="text-center mt-1">{title}</div>
        </Link>
    );
}