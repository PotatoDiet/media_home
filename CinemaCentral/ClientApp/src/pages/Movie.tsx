import React, { useEffect, useRef, useState } from 'react';
import './Movie.css';
import VideoPlayer from '../components/VideoPlayer';
import {useNavigate, useParams} from "react-router-dom";
import {ccFetch} from "../utitilies";

type Genre = {
    name: string;
}

const Video = () => {
  const params = useParams();
  const navigate = useNavigate();
  const [title, setTitle] = useState('');
  const [genres, setGenres] = useState('');
  const [communityRating, setCommunityRating] = useState(0.0);
  const [year, setYear] = useState('');
  const [currentWatchTimestamp, setCurrentWatchTimestamp] = useState(0);

  useEffect(() => {
    const grabData = async () => {
      const [res, watchtime] = await Promise.all([
          ccFetch(`/api/Movies/${params.id}`, "GET", navigate),
          ccFetch(`/api/Movies/${params.id}/GetWatchtimeStamp`, "GET", navigate)
      ]);
      const data = await res.json();

      setTitle(data.title);
      setGenres(data.genres?.map((genre: Genre) => genre.name).join(', '));
      setCommunityRating(data.communityRating);
      setYear(data.year);
      setCurrentWatchTimestamp(await watchtime.json());
    };
    
    grabData();
  }, []);

  if (title === '' || params.id === undefined) {
    return <></>;
  }

  return (
    <div className="video">
      <h1>
        {title}
        {' '}
        (
        {year}
        )
      </h1>
      <div>
        Genres:
        {' '}
        {genres}
      </div>
      <div>
        {communityRating}
        {' '}
        <i className="fas fa-star" />
      </div>

      <VideoPlayer id={params.id} watchtime={currentWatchTimestamp} />
    </div>
  );
};

export default Video;
