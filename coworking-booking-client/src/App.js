import logo from './logo.svg';
import './App.css';
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { AuthProvider } from "./context/AuthContext";
import Login from "./pages/Login";
import Register from "./pages/Register";
import Workspaces from "./pages/Workspaces";
import MyBookings from "./pages/MyBookings";
import Navbar from "./components/Navbar";
import Profile from "./pages/Profile";

function App() {
  return (
    <AuthProvider>
      <Router>
		<Navbar />
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route path="/workspaces" element={<Workspaces />} />
		  <Route path="/bookings" element={<MyBookings />} />
		  <Route path="/users/me" element={<Profile />} />
        </Routes>
      </Router>
    </AuthProvider>
  );
}

export default App;