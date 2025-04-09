import { useContext } from "react";
import { AuthContext } from "../context/AuthContext";
import { useNavigate } from "react-router-dom";
import "./Navbar.css";

const Navbar = () => {
  const { token, logout } = useContext(AuthContext);
  const navigate = useNavigate();

  if (!token) return null; // Скрыть navbar, если не авторизован
  const handleLogout = () => {
    logout();
  navigate("/login");}

  return (
    <nav className="navbar">
      <h3 className="navbar-title" onClick={() => navigate("/")}>
        Coworking Booking
      </h3>
      <div className="navbar-buttons">
        <button onClick={() => navigate("/workspaces")}>Work Spaces</button>
        <button onClick={() => navigate("/bookings")}>My Spaces</button>
		<button onClick={() => navigate("/users/me")}>Profile</button>
        <button onClick={logout}>Выйти</button>
      </div>
    </nav>
  );
};

export default Navbar;