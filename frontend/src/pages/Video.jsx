import React, { useEffect, useRef, useState } from 'react';
import PropTypes from 'prop-types';
import './Video.css';
import VideoPlayer from '../components/VideoPlayer';

const Video = ({ match }) => {
  const isMounted = useRef(true);

  const [id, setId] = useState('');
  const [title, setTitle] = useState('');
  const [genres, setGenres] = useState('');
  const [communityRating, setCommunityRating] = useState(0.0);
  const [year, setYear] = useState('');
  const [currentWatchTimestamp, setCurrentWatchTimestamp] = useState(0);

  useEffect(() => {
    const grabData = async () => {
      const res = await fetch(`http://localhost:1234/video/${match.params.id}`);
      const data = await res.json();

      if (isMounted.current) {
        setId(match.params.id);
        setTitle(data.title);
        setGenres(data.genres);
        setCommunityRating(data.communityRating);
        setYear(data.year);
        setCurrentWatchTimestamp(data.currentWatchTimestamp);
      }
    };
    grabData();

    return () => {
      isMounted.current = false;
    };
  });

  if (id === '') {
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
        {genres}
      </div>
      <div>
        {communityRating}
        {' '}
        <i className="fas fa-star" />
      </div>

      <VideoPlayer
        id={id}
        watchtime={currentWatchTimestamp}
      />
    </div>
  );
};

Video.propTypes = {
  match: PropTypes.shape({
    params: PropTypes.shape({
      id: PropTypes.string.isRequired,
    }),
  }).isRequired,
};

export default Video;
