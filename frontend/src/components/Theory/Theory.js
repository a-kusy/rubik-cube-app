import Sidebar from "../Sidebar/Sidebar";
import { Accordion, AccordionItem, AccordionItemHeading, AccordionItemButton, AccordionItemPanel } from 'react-accessible-accordion';
import 'react-accessible-accordion/dist/fancy-example.css';
import { useState, useEffect } from "react";
import axios from "axios";

const Theory = () => {
    const [tutorials, setTutorials] = useState([])

    useEffect(() => {
        getTheory();
    }, []);

    const getTheory = async ()=>{
        try {
            const config = {
                method: 'get',
                url: 'http://localhost:5124/Tutorials', 
                headers: { 'Content-Type': 'application/json'}
            }

            const { data: res } = await axios(config)
            setTutorials(res)

        } catch (error) {
            if (error.response && error.response.status >= 400 && error.response.status <= 500) {
                console.log(error.response)
            }
        }
    }

    return (
        <div className="main-cont">
            <Sidebar></Sidebar>
            <div className="right-cont">
                <h1>Teoria</h1>
                <Accordion allowMultipleExpanded="true" allowZeroExpanded="true">
                    {tutorials.map((tutorial) => (
                        <AccordionItem key={tutorial.id}>
                        <AccordionItemHeading>
                            <AccordionItemButton>
                                {tutorial.name}
                            </AccordionItemButton>
                        </AccordionItemHeading>
                        <AccordionItemPanel>
                                {tutorial.sections.map((section)=>(
                                    <div>
                                        <h3>{section.name}</h3>
                                        <p key={section.id}> {section.content} </p>
                                    </div>        
                                ))}
                        </AccordionItemPanel>
                    </AccordionItem>
                    ))}
                </Accordion>
            </div>
        </div>
        );
}
export default Theory;