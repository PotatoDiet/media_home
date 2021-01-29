import { useHistory } from "react-router-dom";
import './Navbar.css';

function Navbar() {
  const history = useHistory();

  function search(event) {
    // onSearch doesn't work on firefox, so this is the best we can do.
    if (event.key === "Enter") {
      if (event.target.value === "") {
        history.push("/videos");
      } else {
        history.push("/videos?search=" + event.target.value);
      }
    }
  }

  return (
    <nav className="navbar">
      <a href="/">Home</a>
      <a href="/videos">Videos</a>
      <input type="search" placeholder="Search" onKeyPress={search} />
    </nav>
  );
}

export default Navbar;
