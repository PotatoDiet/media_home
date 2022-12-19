import {useLocation, useNavigate} from "react-router-dom";
import {useQuery} from "react-query";
import {ccFetch} from "../utitilies";
import React, {useEffect} from "react";
import MediaTile from "../components/MediaTile";

type Series = {
    id: string;
    posterPath: string;
    mediaType: string;
}

export default function TV() {
    const navigate = useNavigate();
    const { search } = useLocation();

    const { isLoading, data, refetch } = useQuery("mediaData", async () => {
        const response = await ccFetch(`/api/Series${search}`, "GET", navigate);
        return await response.json();
    });

    useEffect(() => {
        refetch();
    }, [search])

    if (isLoading) return <>Loading...</>;

    return (
        <div className="flex flex-wrap">
            {data.map((s: Series) => (
                <MediaTile
                    key={s.id}
                    id={s.id}
                    type={s.mediaType}
                    poster={s.posterPath}
                />
            ))}
        </div>
    );
};