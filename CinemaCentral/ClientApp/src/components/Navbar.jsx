import React, {useEffect, useState} from 'react';
import { useHistory, Link } from 'react-router-dom';
import './Navbar.css';

export default function Navbar() {
  const history = useHistory();
  const [search, setSearch] = useState("");
  
  useEffect(() => {
    const searchQuery = new URLSearchParams(window.location.search).get("search");
    console.log(searchQuery);
    setSearch(searchQuery);
  }, [window.location.search]);

  const onSearch = (event) => {
    // onSearch doesn't work on firefox, so this is the best we can do.
    if (event.key === 'Enter') {
      if (event.target.value === '') {
        history.push('/movies');
      } else {
        history.push(`/movies?search=${event.target.value}`);
      }
    }
  };

  return (
    <nav className="navbar">
      <Link to="/">Home</Link>
      <Link to="/movies">Movies</Link>
      <input type="search" placeholder="Search" onKeyPress={onSearch} defaultValue={search} />
    </nav>
  );
};
