/* eslint-disable jsx-a11y/media-has-caption */

import React, {useEffect, useRef} from 'react';

type VideoPlayerProps = {
    id: string;
    watchtime: number;
}

export default function VideoPlayer({id, watchtime}: VideoPlayerProps) {
    const ref = useRef<HTMLVideoElement>(null);

    useEffect(() => {
        const listener = setInterval(() => {
            // @ts-ignore
            const currWatchtime = Math.floor(ref.current.currentTime ?? 0);
            fetch(`/api/Movies/${id}/UpdateWatchTimestamp`, {
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
    });

    return (
        <video
            width="1024"
            controls
            onLoadStart={() => {
                // @ts-ignore
                ref.current.currentTime = watchtime;
            }}
            ref={ref}
        >
            <source src={`/api/Movies/${id}/Stream`}/>
            Can not play this video
        </video>
    );
}
