import * as React from 'react';
import Sidebar from "../Sidebar/Sidebar";
import Table from './TableModule/TableComponent';
import axios from 'axios';
import { useState } from "react"
import { useEffect } from 'react';


const Ranking = () => {
    const [rankings, setRankings] = useState([])
    
    useEffect(() => {
        const getScores= async () => {
            try {
                const { data: res } = await axios.get('http://localhost:5124/Rankings/format')
                setRankings(res)
                console.log(rankings)
                

            } catch (error) {
                if (error.response && error.response.status >= 400 && error.response.status <= 500) {
                    console.text("problem")
                }
            }
        }
        getScores();
    },[rankings])
    
    return(
        <div className="main-cont">
            <Sidebar></Sidebar>
            <div className="right-cont">
                <h1>Ranking</h1>
                <Table data={rankings} rowsPerPage={5}/>
                
            </div>
        </div>
    )
}
export default Ranking;