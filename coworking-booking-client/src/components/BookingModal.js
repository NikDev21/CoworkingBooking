import React, { useContext, useEffect, useState } from "react";
import axios from "axios";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import { AuthContext } from "../context/AuthContext";
import "./BookingModal.css";

const BookingModal = ({ workspace, onClose, onBook }) => {
  const { userId } = useContext(AuthContext);

  const [date, setDate] = useState(new Date());
  const [occupiedSlots, setOccupiedSlots] = useState([]);
  const [timeSlots, setTimeSlots] = useState([]);
  const [selectedStart, setSelectedStart] = useState(null);
  const [selectedEnd, setSelectedEnd] = useState(null);

  // Загрузка занятых слотов
  useEffect(() => {
    if (!date || !workspace?.id) return;

    const fetchOccupied = async () => {
      try {
        const res = await axios.get("http://localhost:5228/api/bookings/occupied", {
          params: {
            workspaceId: workspace.id,
            date: date.toISOString().split("T")[0],
          },
        });

        const occupied = res.data.map(({ startDate, endDate }) => ({
          start: new Date(startDate),
          end: new Date(endDate),
        }));

        setOccupiedSlots(occupied);
      } catch (err) {
        console.error("Ошибка загрузки занятых слотов", err);
      }
    };

    fetchOccupied();
  }, [date, workspace]);

  // Генерация всех слотов с шагом 30 минут (08:00 – 18:00)
  useEffect(() => {
    if (!date) return;

    const slots = [];
    const base = new Date(date);
    base.setHours(8, 0, 0, 0);

    for (let i = 0; i < 20; i++) {
      const slot = new Date(base.getTime() + i * 30 * 60000);
      slots.push(slot);
    }

    setTimeSlots(slots);
  }, [date]);

  const handleSlotClick = (slot) => {
    if (occupiedSlots.some((b) => slot >= b.start && slot < b.end)) return;

    // Если уже выбран диапазон — сброс
    if (selectedStart && selectedEnd) {
      setSelectedStart(slot);
      setSelectedEnd(null);
      return;
    }

    // Первый клик
    if (!selectedStart) {
      setSelectedStart(slot);
      return;
    }

    // Второй клик — завершение диапазона
    if (slot > selectedStart) {
      setSelectedEnd(slot);
    } else {
      setSelectedStart(slot); // перезаписать старт, если кликнули раньше
    }
  };

  const getPrice = () => {
    if (!selectedStart || !selectedEnd) return 0;
    const diff = (selectedEnd - selectedStart) / (1000 * 60); // в минутах
    return (diff / 30) * 5; // 5 € за 30 минут
  };


  const toLocalISOString = (date) => {
	const pad = (num) => String(num).padStart(2, '0');
	return `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())}T${pad(date.getHours())}:${pad(date.getMinutes())}:00`;
};

  const handleSubmit = () => {
  if (!selectedStart || !selectedEnd)
	  return alert("Выберите начальное и конечное время.");
  
  
	console.log("START:", selectedStart);
	console.log("END:", selectedEnd);

    const bookingData = {
      userId,
      workspaceId: workspace.id,
      startDate: toLocalISOString(selectedStart),
	  endDate: toLocalISOString(selectedEnd),
    };

    onBook(bookingData);
    onClose();
  };

  return (
    <div className="modal">
      <div className="modal-content">
        <div className="form-section">
          <h3>Бронирование: {workspace.name}</h3>
          <p><strong>Локация:</strong> {workspace.location}</p>

          <label>Дата:</label>
          <DatePicker
            selected={date}
            onChange={setDate}
            dateFormat="yyyy-MM-dd"
            className="datepicker"
          />

          <p>Стоимость: <strong>{getPrice()} €</strong></p>

          <button onClick={handleSubmit}>Забронировать</button>
          <button onClick={onClose}>Отмена</button>
        </div>

        <div className="slots-section">
          {timeSlots.map((slot, index) => {
            const isOccupied = occupiedSlots.some(
              (b) => slot >= b.start && slot < b.end
            );

            const isSelected =
              (selectedStart &&
                !selectedEnd &&
                slot.getTime() === selectedStart.getTime()) ||
              (selectedStart &&
                selectedEnd &&
                slot >= selectedStart &&
                slot < new Date(selectedEnd.getTime() + 30 * 60 * 1000));

            return (
              <div
                key={index}
                className={
                  isOccupied
                    ? "slot occupied"
                    : isSelected
                    ? "slot selected"
                    : "slot free"
                }
                onClick={() => handleSlotClick(slot)}
              >
                {slot.toLocaleTimeString([], {
                  hour: "2-digit",
                  minute: "2-digit",
                })}
              </div>
            );
          })}
        </div>
      </div>
    </div>
  );
};

export default BookingModal;
