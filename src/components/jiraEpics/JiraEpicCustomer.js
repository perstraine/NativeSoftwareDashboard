import styles from "./JiraEpicSection.module.css";
import { useState, useEffect, useRef } from "react";

export default function JiraEpicCustomer({ epic }) {
  const [isOpen, setIsOpen] = useState(false);
    function addComment() {
        console.log("EEWE")
    }
  let refUrl = useRef();
  useEffect(() => {
    let handler = (event) => {
      if (!refUrl.current.contains(event.target)) {
        setIsOpen(false);
      }
    };
    document.addEventListener("mousedown", handler);
    return () => {
      document.removeEventListener("mousedown", handler);
    };
  });
  function handleClick() {
    setIsOpen(!isOpen);
  }
  return (
    <div id={styles.epic} statuscolor={epic.urgencyColour}>
      <div id={styles.name}>{epic.name}</div>
      <div id={styles.start}>{epic.startDate.slice(0, 10)}</div>
      <div id={styles.due}>{epic.dueDate.slice(0, 10)}</div>
      <div id={styles.complete}>{`${epic.complete}%`}</div>
      <div id={styles.extra2} onClick={handleClick} ref={refUrl}>
        <div className={styles.dot} statuscolor={epic.urgencyColour}></div>
        <div className={styles.dot} statuscolor={epic.urgencyColour}></div>
        <div className={styles.dot} statuscolor={epic.urgencyColour}></div>
        {isOpen && (
          <div id={styles.dropdown}>
            <div>
              <a
                href={epic.url}
                target="_blank"
                className={styles.dropdownItem}
              >
                View in Jira
              </a>
            </div>
            <div className={styles.dropdownItem} onClick={addComment}>Comment</div>
          </div>
        )}
      </div>
    </div>
  );
}
