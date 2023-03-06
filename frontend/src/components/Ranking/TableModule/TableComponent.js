import React, { useState } from "react";

import useTable from "./useTable";
import styles from "./Table.module.css";
import TableFooter from "./TableFooter";

const Table = ({ data, rowsPerPage }) => {
    const [page, setPage] = useState(1);
    const { slice, range } = useTable(data, page, rowsPerPage);
    return (
        <div>
            <table className={styles.table}>
                <thead className={styles.tableRowHeader}>
                    <tr>
                        <th className={styles.tableHeader}>Pozycja</th>
                        <th className={styles.tableHeader}>Nazwa Użytkownika</th>
                        <th className={styles.tableHeader}>Czas układania</th>
                        <th className={styles.tableHeader}>Data ułożenia</th>
                    </tr>
                </thead>
                <tbody>
                    {slice.map((el) => (
                        <tr className={styles.tableRowItems} key={el.id}>
                            <td className={styles.tableCell}>{el.position}</td>
                            <td className={styles.tableCell}>{el.username}</td>
                            <td className={styles.tableCell}>{el.time / 1000}s</td>
                            <td className={styles.tableCell}>{new Date(el.date).toISOString().split("T")[0]}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
            <TableFooter range={range} slice={slice} setPage={setPage} page={page} />
        </div>
    );

};

export default Table;