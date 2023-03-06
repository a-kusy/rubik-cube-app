import Sidebar from "../Sidebar/Sidebar"
import ToggleSwitch from "./ToggleSwitch/ToggleSwitch";
import "./Cube.css";
import { Unity, useUnityContext } from "react-unity-webgl";
import {useEffect, useState, useCallback } from "react";
import Popup from "reactjs-popup";
import Stopwatch from "./Stopwatch/Stopwatch";

const Cube = () => {
    const [isRotated, setIsRotated] = useState(false);
    const [isSolved, setIsSolved] = useState(false);
    const [stopwatchEnabled, setStopwatchEnabled] = useState(false);
    const [hintsEnabled, setHintsEnabled] = useState(false);

    const { unityProvider, UNSAFE__detachAndUnloadImmediate: detachAndUnloadImmediate, 
        sendMessage, addEventListener, removeEventListener } = useUnityContext({
        loaderUrl: "Build/WebGL.loader.js",
        dataUrl: "Build/WebGL.data",
        frameworkUrl: "Build/WebGL.framework.js",
        codeUrl: "Build/WebGL.wasm",
    });

    const handleIsRotated = useCallback(() => {
        setIsRotated(true);
    },[]);

    const handleIsSolved = useCallback(() => {
        setIsSolved(true);
    },[]);

    useEffect(() => {
        addEventListener("FirstMove", handleIsRotated)
        addEventListener("Solved", handleIsSolved)
        return () => {
          detachAndUnloadImmediate().catch((reason) => {
            console.log(reason);
          });
          removeEventListener("FirstMove", handleIsRotated);
          removeEventListener("Solved", handleIsSolved);
        };
      }, [detachAndUnloadImmediate, addEventListener, 
        removeEventListener, handleIsRotated, handleIsSolved])

    function handleResetButton() {
        sendMessage("Cube", "Solver");
    }

    function handleShuffleButton() {
        sendMessage("Cube", "Shuffle");
    }

    function handleHintButton() {
        sendMessage("Cube", "Hint");
    }

    function popup() {
        return (
            <form className="form-cont">
                Podpowiedzi: &nbsp;
                <ToggleSwitch Name="Hints" onClick={() => 
                    {
                        if(window.confirm("Włączenie podpowiedzi spowoduje wyłączenie czasomierza.")){
                            setHintsEnabled(!hintsEnabled)
                            setStopwatchEnabled(!stopwatchEnabled) 
                        }
                    }}/><br /><br />
                Czasomierz: &nbsp;
                <ToggleSwitch Name="Timer" onClick={() => setStopwatchEnabled(!stopwatchEnabled)}></ToggleSwitch><br /><br />

                <input className="popupBtn" type="reset" name="Reset" value="Przywróć domyślne"></input>
            </form>
        )
    }

    return (
        <div className="main-cont">
            <Sidebar></Sidebar>
            <div className="right-cont">
                <h1>Układanie kostki</h1>
                <div style={{ position: 'absolute', width: '300px' }}>
                    <Popup trigger={
                        <button className="button" onClick={popup}>Ustawienia</button>} position="right top">
                        {popup}
                    </Popup>
                    <button className="button" onClick={handleResetButton}>Resetuj</button>
                    <button className="button" onClick={handleShuffleButton}>Wymieszaj</button>
                    {hintsEnabled && <button className="button" onClick={handleHintButton}>Podpowiedz</button>}
                </div>
                <Unity unityProvider={unityProvider} style={{ width: '100%', height: '75%' }} />
                {!hintsEnabled && stopwatchEnabled && <Stopwatch isRotated={isRotated} isSolved={isSolved}></Stopwatch>}
            </div>
        </div>
    )
}
export default Cube