import React from 'react';
import './Video.css';
import VideoPlayer from "../components/VideoPlayer";

class Video extends React.Component {
  constructor() {
    super();
    this.state = {
      id: "",
      title: "",
      genres: "",
      year: "",
      currentWatchTimestamp: 0
    }
  }

  async componentDidMount() {
    const id = this.props.match.params.id
    const res = await fetch(`http://localhost:1234/video/${id}`);
    const data = await res.json();
    this.setState({
      id: id,
      title: data.title,
      genres: data.genres,
      communityRating: data.communityRating,
      year: data.year,
      currentWatchTimestamp: data.currentWatchTimestamp
    });
  }

  render() {
    if (this.state.id === "") {
      return <div />
    };

    return (
      <div className="video">
        <h1>{this.state.title} ({this.state.year})</h1>
        <div>Genres: {this.state.genres}</div>
        <div>{this.state.communityRating} <i className="fas fa-star"></i></div>

        <VideoPlayer id={this.state.id}
                     watchtime={this.state.currentWatchTimestamp}/>
      </div>
    );
  }
}

export default Video;
