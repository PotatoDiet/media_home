import React from 'react';

class Video extends React.Component {
  render() {
    return <p>{this.props.title} ({this.props.year})</p>
  }
}
