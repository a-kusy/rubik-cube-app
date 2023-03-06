import { useEffect, useState } from 'react'
import "./Stopwatch.scss"
import axios from "axios"
import Cookies from 'js-cookie'

const Stopwatch = ({ isRotated, isSolved }) => {
  const [elapsedTime, setElapsedTime] = useState(0);
  const [intervalId, setIntervalId] = useState(null);

  const token = Cookies.get("token")

  function setStatistics(elapsedTime) {
    if (token) {
      try {
        const config = {
          method: 'post',
          url: 'http://localhost:5124/Scores',
          headers: { 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + token },
          data: {
            "time": elapsedTime,
            "date": new Date(),
          }
        }

        axios(config)

      } catch (error) {
        if (error.response && error.response.status >= 400 && error.response.status <= 500) {
          console.text(error.response.data.message)
        }
      }
    } else {
      var localStatistics = JSON.parse(localStorage.getItem("statistics"))
      if (localStatistics === null || localStatistics === undefined) localStatistics = [];
      localStatistics.push({ date: new Date(), time: elapsedTime })
      localStorage.setItem("statistics", JSON.stringify(localStatistics))
    }
  }

  useEffect(() => {
    if (isRotated) {
      if (intervalId === null) {
        let startTime = Date.now();
        const id = setInterval(() => {
          setElapsedTime(Date.now() - startTime);
        }, 1);
        setIntervalId(id);
      }
    } else {
      clearInterval(intervalId);
      setIntervalId(null);
    }

    if(isSolved){
      setStatistics(elapsedTime)
      clearInterval(intervalId);
      setIntervalId(null);
    }
  }, [isRotated, intervalId, isSolved, elapsedTime]);

  const elapsed = new Date(elapsedTime);
  const hours = elapsed.getUTCHours();
  const minutes = elapsed.getUTCMinutes();
  const seconds = elapsed.getUTCSeconds();
  const milliseconds = elapsed.getUTCMilliseconds();

  return (
    <div className='wrapper'>
      {hours}:{minutes}:{seconds}.{milliseconds}
    </div>
  );
}
export default Stopwatch;