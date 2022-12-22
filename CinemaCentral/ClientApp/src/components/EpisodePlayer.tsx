import React, {useEffect, useRef} from "react";

type VideoPlayerProps = {
    id: string;
    watchtime: number;
}

export default function EpisodePlayer({id, watchtime}: VideoPlayerProps) {
    const ref = useRef<HTMLVideoElement>(null);

    useEffect(() => {
        const listener = setInterval(() => {
            const currWatchtime = Math.floor(ref.current?.currentTime ?? 0);
            fetch(`/api/Series/UpdateWatchTimestamp/${id}`, {
                method: "POST",
                body: JSON.stringify(currWatchtime),
                headers: {
                    "Content-Type": "application/json"
                },
            });
        }, 5000);

        return () => {
            clearInterval(listener);
        };
    }, []);

    return (
        <video
            width="1024"
            controls
            onLoadStart={() => {
                if (ref.current) {
                    ref.current.currentTime = watchtime;
                }
            }}
            ref={ref}
            autoPlay
        >
            <source src={`/api/Series/GetEpisodeStream/${id}`}/>
            Can not play this video
        </video>
    );
}