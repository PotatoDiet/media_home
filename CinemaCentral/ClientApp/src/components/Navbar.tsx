import React, {useEffect, useState} from 'react';
import { useNavigate, Link } from 'react-router-dom';
import './Navbar.css';

export default function Navbar() {
  const navigate = useNavigate();
  const [search, setSearch] = useState("");
  
  useEffect(() => {
    const searchQuery = new URLSearchParams(window.location.search).get("search");
    setSearch(searchQuery ?? "");
  }, [window.location.search]);

  const onSearch = (event: React.KeyboardEvent<HTMLInputElement>) => {
    
    // onSearch doesn't work on firefox, so this is the best we can do.
    if (event.key === 'Enter') {
      const target = event.target as HTMLInputElement;
      
      if (target.value === 'enter') {
        navigate('/movies');
      } else {
        navigate(`/movies?search=${target.value}`);
      }
    }
  };

  return (
    <nav className="navbar">
      <Link to="/">Home</Link>
      <Link to="/movies">Movies</Link>
      <input type="search" placeholder="Search" onKeyUp={onSearch} defaultValue={search} />
    </nav>
  );
};
