import React from 'react';
import { Switch, Route } from 'react-router-dom';
import Navbar from './components/Navbar';
import Home from './pages/Home';
import Movies from './pages/Movies';
import Movie from './pages/Movie';
import './App.css';

const App = () => (
  <div>
    <Navbar />

    <div className="content">
      <Switch>
        <Route exact path="/" component={Home} />
        <Route path="/movies" component={Movies} />
        <Route path="/movie/:id" component={Movie} />
      </Switch>
    </div>
  </div>
);

export default App;
