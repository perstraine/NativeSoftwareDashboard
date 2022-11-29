import styles from "./JiraEpicSection.module.css";
import { useState, useEffect, useRef } from "react";
import AddJiraComment from "./AddJiraComment";


export default function JiraEpicCustomer({ epic }) {
    const [isOpen, setIsOpen] = useState(false);
  const [addJiraCommentPopup, setAddJiraCommentPopup] = useState(false);
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
      <AddJiraComment
        trigger={addJiraCommentPopup}
              setTrigger={setAddJiraCommentPopup}
              issue = {epic.id}
      ></AddJiraComment>
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
            <div className={styles.dropdownItem} onClick={()=>{setAddJiraCommentPopup(true)}}>
              Comment
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
