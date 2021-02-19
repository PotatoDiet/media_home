import React, { useEffect, useState } from "react";
import "./Video.css";
import VideoPlayer from "../components/VideoPlayer";

const Video = props => {
  const [id, setId] = useState("");
  const [title, setTitle] = useState("");
  const [genres, setGenres] = useState("");
  const [communityRating, setCommunityRating] = useState(0.0);
  const [year, setYear] = useState("");
  const [currentWatchTimestamp, setCurrentWatchTimestamp] = useState("");

  useEffect(() => {
    const grabData = async () => {
      const id = props.match.params.id
      const res = await fetch(`http://localhost:1234/video/${id}`);
      const data = await res.json();

      setId(id);
      setTitle(data.title);
      setGenres(data.genres);
      setCommunityRating(data.communityRating);
      setYear(data.year);
      setCurrentWatchTimestamp(data.currentWatchTimestamp);
    }
    grabData();
  }, []);

  if (id === "") {
    return <div />
  }

  return (
    <div className="video">
      <h1>{title} ({year})</h1>
      <div>Genres: {genres}</div>
      <div>{communityRating} <i className="fas fa-star"></i></div>

      <VideoPlayer id={id}
                   watchtime={currentWatchTimestamp}/>
    </div>
  );
};

export default Video;
