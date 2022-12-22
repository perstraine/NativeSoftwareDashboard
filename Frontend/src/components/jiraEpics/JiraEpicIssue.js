import styles from "./JiraEpicSection.module.css";
import { useState, useEffect, useRef } from "react";

export default function JiraEpicIssue({ epic }) {
  const [isOpen, setIsOpen] = useState(false);
  
  let refUrl = useRef();
  useEffect(() => {
    let handler = (event) =>{
      if(!refUrl.current.contains(event.target)){
        setIsOpen(false);
      }
    }
    document.addEventListener("mousedown", handler);
    return() => {
      document.removeEventListener("mousedown", handler);
    }
  })
  function handleClick() {
    setIsOpen(!isOpen)
  }
    return (
      <div id={styles.epic} statuscolor={epic.urgencyColour}>
        <div id={styles.account}>{epic.account}</div>
        <div id={styles.name}>{epic.name}</div>
        <div id={styles.start}>{epic.startDate.slice(0, 10)}</div>
        <div id={styles.due}>{epic.dueDate.slice(0, 10)}</div>
        <div id={styles.story}>{epic.storyPoints}</div>
        <div id={styles.budget}>{epic.budget}</div>
        <div id={styles.spent}>{epic.timeSpent}</div>
        <div id={styles.complete}>{`${epic.complete}%`}</div>
        <div id={styles.extra2} onClick={handleClick} ref={refUrl}>
          <div className={styles.dot} statuscolor={epic.urgencyColour}></div>
          <div className={styles.dot} statuscolor={epic.urgencyColour}></div>
          <div className={styles.dot} statuscolor={epic.urgencyColour}></div>
          {isOpen && <div id={styles.dropdown}><a href={epic.url} target="_blank" className={styles.dropdownItem}>View in Jira</a></div>}
        </div>
      </div>
    );
}