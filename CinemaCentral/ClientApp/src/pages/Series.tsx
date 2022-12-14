import {useParams} from "react-router-dom";
import React, {useEffect, useState} from "react";
import EpisodeTile from "../components/EpisodeTile";

type Season = {
    seasonNumber: number;
    episodes: Episode[];
}

type Episode = {
    id: string;
    posterPath: string;
    title: string;
    seasonNumber: number;
    episodeNumber: number;
}

export default function Series() {
    const params = useParams();
    
    let [seasons, setSeasons] = useState([] as Season[]);

    useEffect( () => {
        async function grabVideos() {
            const res = await fetch(`/api/Series/GetSeries/${params.id}`);
            const data = await res.json();
            
            const seasonsDict: { [key: number]: Episode[] } = {};
            data["episodes"].forEach((episode: Episode) => {
                if (episode.seasonNumber in seasonsDict) {
                    seasonsDict[episode.seasonNumber].push(episode);
                } else {
                    seasonsDict[episode.seasonNumber] = [episode];
                }
            });
            
            const seasonsArr: Season[] = [];
            for (const [key, value] of Object.entries(seasonsDict)) {
                value.sort((a, b) => a.episodeNumber - b.episodeNumber);
                seasonsArr.push({ seasonNumber: parseInt(key), episodes: value });
            }
            seasonsArr.sort((a, b) => a.seasonNumber - b.seasonNumber);
            setSeasons(seasonsArr);
        }
        grabVideos();
    }, [window.location.search]);
    
    return (
        <>
            {seasons.map((s: Season) => (
                <div className="flex flex-wrap mb-10" key={s.seasonNumber}>
                    <h2 className="w-full m-3 text-2xl">Season {s.seasonNumber}</h2>

                    {s.episodes.map((e: Episode) => (
                        <EpisodeTile
                            key={e.id}
                            id={e.id}
                            poster={e.posterPath}
                            title={e.title}
                            episodeNumber={e.episodeNumber}
                        />
                    ))}
                </div>
            ))}
        </>
    );
}