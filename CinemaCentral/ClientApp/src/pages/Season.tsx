import React, {useState} from "react";
import {useNavigate, useParams} from "react-router-dom";
import {useQuery} from "react-query";
import {ccFetch} from "../utitilies";
import EpisodeTile from "../components/EpisodeTile";

type SeasonT = {
    number: number;
    episodes: Episode[];
}

type Episode = {
    id: string;
    episodeNumber: number;
    title: string;
    posterPath: string;
}

export default function Season() {
    const { id } = useParams();
    const navigate = useNavigate();
    const [season, setSeason] = useState<SeasonT | undefined>(undefined);

    useQuery("getSeason", {
        queryFn: () => {
            return ccFetch(`/api/Series/getSeason/${id}`, "GET", navigate);
        },
        onSuccess: async (data: Response) => {
            const tmpSeason = await data.json() as SeasonT;
            tmpSeason.episodes.sort((a, b) => a.episodeNumber - b.episodeNumber);
            setSeason(tmpSeason);
        }
    });
    
    return (
        <>
            <h1>{season?.number}</h1>

            <div className="flex flex-wrap">
                {season?.episodes.map((e) => (
                    <EpisodeTile
                        key={e.id}
                        id={e.id}
                        title={e.title}
                        episodeNumber={e.episodeNumber}
                        poster={e.posterPath}
                    />
                ))}
            </div>
        </>
    )
}