import { useContext, useEffect, useState } from "react";
import axios from "axios";
import { AuthContext } from "../context/AuthContext";
import { useNavigate } from "react-router-dom";
import BookingModal from "../components/BookingModal";
import "./Workspaces.css";

const Workspaces = () => {
  const [workspaces, setWorkspaces] = useState([]);
  const [selectedWorkspace, setSelectedWorkspace] = useState(null);
  const { token } = useContext(AuthContext);
  const navigate = useNavigate();

  useEffect(() => {
    if (!token) return navigate("/login");

    const load = async () => {
      const updated = await fetchUpdatedWorkspaces();
      setWorkspaces(updated);
    };

    load();
  }, [token, navigate]);

  const fetchUpdatedWorkspaces = async () => {
    const res = await axios.get("http://localhost:5228/api/workspaces", {
      headers: { Authorization: `Bearer ${token}` }
    });

    const updated = await Promise.all(res.data.map(async (ws) => {
      const today = new Date().toISOString().split("T")[0];

      const response = await axios.get("http://localhost:5228/api/bookings/occupied", {
        params: {
          workspaceId: ws.id,
          date: today
        },
        headers: { Authorization: `Bearer ${token}` }
      });

      const occupiedSlots = response.data.map(({ startDate, endDate }) => ({
        start: new Date(startDate),
        end: new Date(endDate)
      }));

      const slots = [];
      const base = new Date();
      base.setHours(8, 0, 0, 0);
      for (let i = 0; i < 20; i++) {
        slots.push(new Date(base.getTime() + i * 30 * 60 * 1000));
      }

      const hasFree = slots.some(slot =>
        !occupiedSlots.some(b => slot >= b.start && slot < b.end)
      );

      return { ...ws, isAvailable: hasFree };
    }));

    return updated;
  };

  const handleBooking = async (bookingData) => {
    try {
      await axios.post("http://localhost:5228/api/bookings", bookingData, {
        headers: { Authorization: `Bearer ${token}` }
      });

      alert("Бронирование успешно!");

      const updated = await fetchUpdatedWorkspaces();
      setWorkspaces(updated);
    } catch (err) {
      alert(err.response?.data || "Ошибка бронирования");
    }
  };

  return (
    <div className="workspaces-container">
      <h2>Рабочие места</h2>
      <div className="workspace-grid">
        {workspaces.map((ws) => (
          <div
            key={ws.id}
            className={`workspace-card ${ws.isAvailable ? "free" : "busy"}`}
            onClick={() => ws.isAvailable && setSelectedWorkspace(ws)}
          >
            <h4>{ws.name}</h4>
            <p>{ws.location}</p>
            <strong>{ws.isAvailable ? "Свободно" : "Занято"}</strong>
          </div>
        ))}
      </div>

      {selectedWorkspace && (
        <BookingModal
          workspace={selectedWorkspace}
          onClose={() => setSelectedWorkspace(null)}
          onBook={handleBooking}
        />
      )}
    </div>
  );
};

export default Workspaces;
