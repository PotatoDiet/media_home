import React, {useEffect, useState} from "react";
import {useLocation, useNavigate, useParams} from "react-router-dom";
import {useQuery} from "react-query";
import {ccFetch} from "../utitilies";
import MediaTile from "../components/MediaTile";
import {Media} from "../responseTypes";

export default function Library() {
    const navigate = useNavigate();
    const { search } = useLocation();
    const { id } = useParams();
    const [media, setMedia] = useState([] as Media[]);

    const { refetch } = useQuery(["libraryData"], {
        queryFn: () => {
            return ccFetch(`/api/Library/GetLibraryMedia/${id}${search}`, "GET", navigate);
        },
        onSuccess: async (data: Response) => {
            setMedia(await data.json());
        }
    });
    
    useEffect(() => { refetch() }, [id]);

    return (
        <div className="flex flex-wrap">
            {media.map((v) => (
                <MediaTile
                    key={v.id}
                    id={v.id}
                    type={v.mediaType}
                    poster={v.posterPath}
                />
            ))}
        </div>
    );
}