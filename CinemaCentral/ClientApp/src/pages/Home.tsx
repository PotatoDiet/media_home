import React, {useEffect, useState} from "react";
import MovieTile from "../components/MovieTile";
import './Movies.css';

type Media = {
    id: string;
    posterPath: string;
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
                    <MovieTile
                        key={v.id}
                        id={v.id}
                        poster={v.posterPath}
                    />
                ))}
            </span>
        </div>
    );
};
