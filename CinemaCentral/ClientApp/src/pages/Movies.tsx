import React, { useEffect, useState } from 'react';
import MediaTile from '../components/MediaTile';
import {ccFetch} from "../utitilies";
import {Outlet, useLocation, useNavigate} from "react-router-dom";
import {useQuery} from "react-query";

type Movie = {
  id: string;
  posterPath: string;
}

export default function Movies() {
  const navigate = useNavigate();
  const { search } = useLocation();

  const { isLoading, data, refetch } = useQuery("moviesData", async () => {
    const response = await ccFetch(`/api/Movies${search}`, "GET", navigate);
    return await response.json();
  });
  
  useEffect(() => {
    refetch();
  }, [search])

  if (isLoading) return <>Loading...</>;

  return (
      <div className="flex flex-wrap">
          {data.map((v: Movie) => (
              <MediaTile
                  key={v.id}
                  id={v.id}
                  type="Movie"
                  poster={v.posterPath}
              />
          ))}
      </div>
  );
};
