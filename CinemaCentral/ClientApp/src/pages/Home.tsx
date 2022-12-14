import React, {useEffect, useState} from "react";
import MediaTile from "../components/MediaTile";
import './Movies.css';

type Media = {
    id: string;
    posterPath: string;
    mediaType: string;
}

export default function Home() {
    const [list, setList] = useState([]);

    useEffect( () => {
        async function grabVideos() {
            const res = await fetch(`/api/Media${window.location.search}`);
            setList(await res.json());
        }
        grabVideos();
    }, [window.location.search]);
    
    return (
        <div>
            <span className="videos">
                {list.map((v: Media) => (
                    <MediaTile
                        key={v.id}
                        id={v.id}
                        type={v.mediaType}
                        poster={v.posterPath}
                    />
                ))}
            </span>
        </div>
    );
};
