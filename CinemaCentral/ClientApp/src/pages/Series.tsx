import {useLocation, useNavigate, useParams} from "react-router-dom";
import React, {useEffect, useState} from "react";
import EpisodeTile from "../components/EpisodeTile";
import {ccFetch} from "../utitilies";
import {useQuery} from "react-query";
import Season from "./Season";
import MediaTile from "../components/MediaTile";

type Series = {
    id: string;
    seasons: SeasonT[];
}

type SeasonT = {
    id: string;
    number: number;
    posterPath: string;
}

export default function Series() {
    const { id } = useParams();
    const navigate = useNavigate();
    
    const [series, setSeries] = useState<Series | undefined>(undefined);

    useQuery("getSeries", {
        queryFn: () => {
            return ccFetch(`/api/Series/GetSeries/${id}`, "GET", navigate);
        },
        onSuccess: async (data: Response) => {
            const tmpSeries = await data.json() as Series;
            tmpSeries.seasons.sort((a, b) => a.number - b.number);
            setSeries(tmpSeries);
        }
    });
    
    return (
        <div className="flex flex-wrap">
            {series?.seasons.map((s: SeasonT) => (
                <MediaTile
                    key={s.id}
                    id={s.id}
                    type="Season"
                    poster={s.posterPath}
                />
            ))}
        </div>
    );
}