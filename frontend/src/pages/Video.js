import React from 'react';

class Video extends React.Component {
  constructor() {
    super();
    this.state = {
      id: "",
      title: "",
      year: ""
    }
  }

  async componentDidMount() {
    const id = this.props.match.params.id
    const res = await fetch(`http://localhost:1234/video/${id}`);
    const data = await res.json();
    this.setState({
      id: id,
      title: data.title,
      year: data.year
    });
  }

  render() {
    if (this.state.id === "") {
      return <div />
    };

    return (
      <div>
        <div>Title: {this.state.title}</div>
        <div>Year: {this.state.year}</div>

        <video width="1024" height="728" controls>
          <source src={`http://localhost:1234/stream/${this.state.id}`} type="video/mp4" />
          Can't play this video
        </video>
      </div>
    );
  }
}

export default Video;
