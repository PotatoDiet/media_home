import React, { useEffect, useState } from 'react';
import MediaTile from '../components/MediaTile';
import './Movies.css';
import {ccFetch} from "../utitilies";
import {Outlet, useLocation, useNavigate} from "react-router-dom";
import {useQuery} from "react-query";

type Movie = {
  id: string;
  posterPath: string;
}

export default function Movies() {
  const navigate = useNavigate();

  const { isLoading, error, data } = useQuery("moviesData", async () => {
    const response = await ccFetch(`/api/Movies${window.location.search}`, "GET", navigate);
    return await response.json();
  });

  if (isLoading) return <>Loading...</>;

  return (
      <span className="videos">
          {data.map((v: Movie) => (
              <MediaTile
                  key={v.id}
                  id={v.id}
                  type="Movie"
                  poster={v.posterPath}
              />
          ))}
      </span>
  );
};
