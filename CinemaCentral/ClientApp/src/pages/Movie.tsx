import React, { useEffect, useRef, useState } from 'react';
import './Movie.css';
import VideoPlayer from '../components/VideoPlayer';
import {useParams} from "react-router-dom";

type Genre = {
    name: string;
}

const Video = () => {
  const params = useParams();
  const [title, setTitle] = useState('');
  const [genres, setGenres] = useState('');
  const [communityRating, setCommunityRating] = useState(0.0);
  const [year, setYear] = useState('');
  const [currentWatchTimestamp, setCurrentWatchTimestamp] = useState(0);

  useEffect(() => {
    const grabData = async () => {
      const res = await fetch(`/api/Movies/${params.id}`);
      const data = await res.json();

      setTitle(data.title);
      setGenres(data.genres?.map((genre: Genre) => genre.name).join(', '));
      setCommunityRating(data.communityRating);
      setYear(data.year);
      setCurrentWatchTimestamp(data.currentWatchTimestamp);
    };
    
    grabData();
  });

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
