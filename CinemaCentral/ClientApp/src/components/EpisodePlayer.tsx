import React, {useEffect, useRef} from "react";

type VideoPlayerProps = {
    id: string;
}

export default function EpisodePlayer({id}: VideoPlayerProps) {
    const ref = useRef<HTMLVideoElement>(null);

    return (
        <video
            width="1024"
            controls
        >
            <source src={`/api/Series/GetEpisodeStream/${id}`}/>
            Can not play this video
        </video>
    );
}