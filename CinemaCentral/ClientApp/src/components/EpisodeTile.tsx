import {Link} from "react-router-dom";
import "./EpisodeTile.css";
import React from "react";

type EpisodeTileProps = {
    id: string;
    poster: string;
}

export default function EpisodeTile({ id, poster }: EpisodeTileProps) {
    return (
        <Link
            to={`/episode/${id}`}
            className="episode-tile"
            style={{backgroundImage: `url(${poster})`}}
        />
    );
}