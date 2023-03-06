import Sidebar from "../Sidebar/Sidebar";
import "./Statistics.css"
import CanvasJSReact from './canvasjs.react';
import { useState, useEffect } from "react";
import axios from "axios";
import Cookies from 'js-cookie';

var CanvasJSChart = CanvasJSReact.CanvasJSChart;

const Statistics = () => {
    const [scores, setScores] = useState([])
    const [userRanking, setUserRanking] = useState([])
    const token = Cookies.get("token")

    useEffect(() => {
        getUserScore();
        getUserRanking();
    }, []);

    const getUserScore = async () => {
        if (token) {
            try {
                const config = {
                    method: 'get',
                    url: 'http://localhost:5124/Scores/user',
                    headers: { 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + token }
                }

                const { data: res } = await axios(config)
                setScores(res)

            } catch (error) {
                if (error.response && error.response.status >= 400 && error.response.status <= 500) {
                    console.log(error.response)
                }
            }
        }
        else {
            var localStatistics = JSON.parse(localStorage.getItem("statistics"));
            if(localStatistics !== null && localStatistics !== undefined) {
                setScores(localStatistics)}
        }
    }

    const getUserRanking= async () => {
        if (token) {
            try {
                const config = {
                    method: 'get',
                    url: 'http://localhost:5124/Rankings/user',
                    headers: { 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + token }
                }

                const { data: res } = await axios(config)
                setUserRanking(res)

            } catch (error) {
                if (error.response && error.response.status >= 400 && error.response.status <= 500) {
                    console.log(error.response)
                }
            }
        }
    }

    const options = {
        animationEnabled: true,
        exportEnabled: false,
        backgroundColor: "#1D1D1D",
        theme: "dark1", 
        title: {
            text: ""
        },
        axisY: {
            title: "Czas",
            suffix: "s"
        },
        axisX: 
            getInterval()
        ,
        data: [{
            type: "line", 
            toolTipContent: "{x} - {y}s",
            xValueFormatString: "DD.M.YY H:mm:ss",
            dataPoints: getDataPoints()
        }]
    }

    function getDataPoints() {
        var dataPoints = []
        if (scores.length > 0 ) {
            for (let i = 0; i < scores.length; i++) {
                dataPoints.push({ x: new Date(scores[i].date), y: scores[i].time/1000 });
            }
        }
        
        dataPoints.sort(function (a, b) { return a.x - b.x })
        return dataPoints
    }

    function getInterval(){
        if (scores.length > 0) {
        var date1 = new Date(scores[0].date)
        var date2 = new Date(scores[scores.length - 1].date)
        const diffTime = Math.abs(date2 - date1);
        const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24)); 
        
        var formatString = "DD.M.YY"
        
        if(diffDays <= 3){
            formatString = "DD.M.YY HH:mm"
        }

        return {
            title: "Data",
            valueFormatString: formatString
            }
        }
    }

    function GetBestTime() {
        if (scores.length > 0) {
            const temp = scores
            temp.sort(function (a, b) { return a.time - b.time })
            return temp[0].time/1000
        }
        else return 0
    }

    function GetSpendTime() {
        if (scores.length > 0) {
            var spendTime = 0;

            scores.forEach(score => {
                spendTime += score.time
            });

            return spendTime / 1000
        }
        else return 0
    }

    return (
        <div className="main-cont">
            <Sidebar></Sidebar>
            <div className="right-cont">
                <h1>Statystyki</h1>
                <div className="stat-flex">
                    <div className="statistics">
                        <table>
                            <tbody>
                                <tr>
                                    <th className="left-th">Najlepszy czas</th>
                                    <th><GetBestTime />s</th>
                                </tr>
                                <tr>
                                    <th className="left-th">Czas spędzony na układaniu</th>
                                    <th><GetSpendTime />s</th>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div className="statistics">
                        <table>
                            <tbody>
                                <tr>
                                    <th className="left-th">Ułożeń kostki</th>
                                    <th>{scores.length}</th>
                                </tr>
                                <tr>
                                    <th className="left-th">Pozycja w rankingu</th>
                                    <th>{token == null ? "-" : userRanking.position}</th>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <div id="chart">
                    <CanvasJSChart options={options}/>
                </div>
            </div>
        </div>
    );
}
export default Statistics;